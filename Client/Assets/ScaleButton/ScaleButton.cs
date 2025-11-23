using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class ScaleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	
	[SerializeField] private RectTransform _target; // дочерний объект
	[SerializeField] private ScaleButtonConfig _scaleButtonConfig;
	
	private Vector3 _originalScale;
	private Coroutine _coroutine;

	void Awake()
	{
		if (_target != null)
			_originalScale = _target.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		StartScaleAnimation(_originalScale * _scaleButtonConfig.PressedScale);
		PlaySound(_scaleButtonConfig.PressSound);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		StartScaleAnimation(_originalScale);
		PlaySound(_scaleButtonConfig.ReleaseSound);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StartScaleAnimation(_originalScale);
	}

	private void StartScaleAnimation(Vector3 targetScale)
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);
		_coroutine = StartCoroutine(ScaleAnimation(targetScale));
	}

	private IEnumerator ScaleAnimation(Vector3 targetScale)
	{
		Vector3 start = _target.localScale;
		float t = 0f;

		while (t < 1f)
		{
			t += Time.unscaledDeltaTime / _scaleButtonConfig.Duration;
			_target.localScale = Vector3.Lerp(start, targetScale, t);
			yield return null;
		}

		_target.localScale = targetScale;
	}
	
	private void PlaySound(AudioClip clip)
	{
		if (clip != null)
		{
			if (ScaleButtonAudioManager.Instance)
			{
				ScaleButtonAudioManager.Instance.PlayOneShot(clip, 0.1f);	
			}
		}
	}
	
	//[Button("Fold")]
	[ContextMenu("Fold")]
	public void Fold()
    {
        var srcImage = GetComponent<Image>();

        // 1) Создаём Inside и растягиваем
        GameObject insideGO = new GameObject("Inside", typeof(RectTransform));
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(insideGO, "Create Inside");
        Undo.SetTransformParent(insideGO.transform, transform, "Parent Inside");
#endif
        var insideRT = insideGO.GetComponent<RectTransform>();
        insideRT.anchorMin = Vector2.zero;
        insideRT.anchorMax = Vector2.one;
        insideRT.pivot = new Vector2(0.5f, 0.5f);
        insideRT.anchoredPosition = Vector2.zero;
        insideRT.sizeDelta = Vector2.zero;
        insideRT.localScale = Vector3.one;
        insideRT.localRotation = Quaternion.identity;
        insideGO.transform.SetSiblingIndex(0);

        // 2) Внутри Inside создаём Image и копируем настройки из кнопки
        GameObject imgGO = new GameObject("Image", typeof(RectTransform), typeof(Image));
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(imgGO, "Create Inside Image");
        Undo.SetTransformParent(imgGO.transform, insideGO.transform, "Parent Inside Image");
#else
        imgGO.transform.SetParent(insideGO.transform);
#endif
        var imgRT = imgGO.GetComponent<RectTransform>();
        imgRT.anchorMin = Vector2.zero;
        imgRT.anchorMax = Vector2.one;
        imgRT.pivot = new Vector2(0.5f, 0.5f);
        imgRT.anchoredPosition = Vector2.zero;
        imgRT.sizeDelta = Vector2.zero;
        imgRT.localScale = Vector3.one;
        imgRT.localRotation = Quaternion.identity;

        var dstImage = imgGO.GetComponent<Image>();
        // Копируем основные поля без «кучи проверок»
        if (srcImage != null)
        {
            dstImage.sprite = srcImage.sprite;
            dstImage.type = srcImage.type;
            dstImage.preserveAspect = srcImage.preserveAspect;
            dstImage.fillCenter = srcImage.fillCenter;
            dstImage.fillMethod = srcImage.fillMethod;
            dstImage.fillAmount = srcImage.fillAmount;
            dstImage.fillClockwise = srcImage.fillClockwise;
            dstImage.fillOrigin = srcImage.fillOrigin;
            dstImage.useSpriteMesh = srcImage.useSpriteMesh;
	        //dstImage.alphaHitTestMinimumThreshold = srcImage.alphaHitTestMinimumThreshold;
            dstImage.material = srcImage.material;
            dstImage.raycastTarget = false; // srcImage.raycastTarget
            dstImage.maskable = srcImage.maskable;
            dstImage.color = srcImage.color;
            dstImage.pixelsPerUnitMultiplier = srcImage.pixelsPerUnitMultiplier;
        }

        // 3) Перекладываем все текущие дочерние объекты кнопки внутрь Inside (кроме самого Inside)
        var toMove = new System.Collections.Generic.List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child == insideGO.transform) continue;
            toMove.Add(child);
        }
        foreach (var ch in toMove)
        {
#if UNITY_EDITOR
            Undo.SetTransformParent(ch, insideGO.transform, "Move children into Inside");
#else
            ch.SetParent(insideGO.transform, false);
#endif
        }

        srcImage.sprite = null;
        srcImage.color = new Color(0f, 0f, 0f, 0f);
        
        transform.GetComponent<Button>().targetGraphic = dstImage;
        
        // 4) Делаем Inside target’ом для скейла, запоминаем исходный масштаб
        _target = insideRT;
        _originalScale = Vector3.one;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(gameObject);
        EditorUtility.SetDirty(insideGO);
        EditorUtility.SetDirty(imgGO);
#endif
    }


    // private void SetTargetGraphic()
    // {
    // }
    
}