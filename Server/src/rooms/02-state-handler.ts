import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Vector2Float extends Schema{
    @type("number")
    x = Math.floor(Math.random() * 128) - 64;

    @type("number")
    z = Math.floor(Math.random() * 128) - 64;

    constructor(x: number, z: number){
        super()
        this.x = x;
        this.z = z;
    }
}
export class FoodState extends Schema{
    @type("string")
    type = "Apple";

    @type(Vector2Float)
    position: Vector2Float;

    constructor(type: string, position: Vector2Float){
        super()
        this.type = type;
        this.position = position;
    }
}

export class Player extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 128) - 64;

    @type("number")
    z = Math.floor(Math.random() * 128) - 64;

    @type("uint16")
    score = 0;

    @type("uint8")
    details = 0;

    @type("uint8")
    skin = 0;
    
    @type("string")
    name: string;

    constructor(name: string, skinIndex: number){
        super();

        this.skin = skinIndex;
        this.name = name;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type({map: FoodState})
    foods = new MapSchema<FoodState>();

    gameOverIDs = [];

    CreateFood(type: string, position: Vector2Float){
        const food = new FoodState(type, position);
        this.foods.set(this.CreateUnicId(), food);
    }

    CollectFood(player: Player, data: any){
        console.log(data);

        if (this.foods.get(data.i) === undefined)
            return;

        this.foods.delete(data.i)

        player.score += data.s;
        player.details = Math.round(player.score / 10);
        

        const type = data.t;
        const position = new Vector2Float(
            Math.floor(Math.random() * 256) - 128, 
            Math.floor(Math.random() * 256) - 128            
        );
        this.CreateFood(type, position);
    }

    createPlayer(sessionId: string, data: any) {
        const skinIndex = this.GetNextSkinIndex();
        this.players.set(sessionId, new Player(data, skinIndex));
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        const player = this.players.get(sessionId);
        
        player.x = movement.x;
        player.z = movement.z;
    }

    gameOver(data : any){
        const detailsPositions = JSON.parse(data);
        const clientID = detailsPositions.Id;
        console.log(clientID);
        console.log(data);

        const gameOverId = this.gameOverIDs.find((value) => value === clientID);
        if (gameOverId !== undefined)
            return;

        this.gameOverIDs.push(clientID);
        this.delayClearGameOverIds(clientID);

        if (this.players.has(clientID)){
            this.removePlayer(clientID);   
        }

        for (let i = 0; i < detailsPositions.Ds.length; i++) {
            this.CreateFood("Apple", new Vector2Float(
                detailsPositions.Ds[i].X,
                detailsPositions.Ds[i].Z
            ));
        }
    }
    async delayClearGameOverIds(clientID){
        await new Promise(resolve => setTimeout(resolve, 10000));

        const index = this.gameOverIDs.findIndex((value) => value === clientID);
        if (index <= -1) {
            return;
        }

        this.gameOverIDs.splice(index, 1);
    }

    skinsLenght = 0;
    lastSkinIndex = 0;
    GetNextSkinIndex() {
        const index = this.lastSkinIndex;
        this.lastSkinIndex = (this.lastSkinIndex + 1) % this.skinsLenght;
        return index;
    }

    lastUnicId = 0;
    CreateUnicId(){
        return (this.lastUnicId++).toString();
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 50;
    startAppleFoodCount = 220;
    startAppleSmallFoodCount = 650;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);
        this.setPatchRate(20);

        this.setState(new State());

        this.state.skinsLenght = options.skins;

        this.onMessage("join", (client, data) => {
            this.state.createPlayer(client.sessionId, data);
        });

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("collect", (client, data) => {
            const player = this.state.players.get(client.sessionId);
            this.state.CollectFood(player, data);
        });

        this.onMessage("gameOver", (client, data) => {
            this.state.gameOver(data);
        });

        this.onMessage("ping", (client, data) => {
            client.send("pong", data);
        });

        for (let i = 0; i < this.startAppleFoodCount; i++) {
            const type = "Apple";
            const position = new Vector2Float(
                Math.floor(Math.random() * 256) - 128, 
                Math.floor(Math.random() * 256) - 128
            );
            this.state.CreateFood(type, position);
        }
        for (let i = 0; i < this.startAppleSmallFoodCount; i++) {
            const type = "AppleSmall";
            const position = new Vector2Float(
                Math.floor(Math.random() * 256) - 128, 
                Math.floor(Math.random() * 256) - 128
            );
            this.state.CreateFood(type, position);
        }
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        //this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        if (this.state.players.has(client.sessionId)){
            this.state.removePlayer(client.sessionId);
        }
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
