import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 128) - 64;

    @type("number")
    z = Math.floor(Math.random() * 128) - 64;

    @type("uint8")
    d = Math.floor(Math.random() * 8);

    @type("uint8")
    skin = 0;

    constructor(skinIndex: number){
        super();

        this.skin = skinIndex;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        const skinIndex = this.GetNextSkinIndex();
        this.players.set(sessionId, new Player(skinIndex));
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        const player = this.players.get(sessionId);
        
        player.x = movement.x;
        player.z = movement.z;
    }

    skinsLenght = 0;
    lastSkinIndex = 0;
    GetNextSkinIndex() {
        const index = this.lastSkinIndex;
        this.lastSkinIndex = (this.lastSkinIndex + 1) % this.skinsLenght;
        return index;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 50;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);
        this.setPatchRate(20);

        this.setState(new State());

        this.state.skinsLenght = options.skins;

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
