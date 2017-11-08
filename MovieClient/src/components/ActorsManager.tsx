import * as React from 'react'
import * as ReactDOM from 'react-dom'
import { RouteComponentProps } from 'react-router'
import * as Models from "../Models"
import {Actor} from './Actor'

import {
    BrowserRouter as Router,
    Route,
    Link
} from 'react-router-dom'

async function loadActors(): Promise<Models.Actor[]> {
    let result = await fetch('http://localhost:5000/api/actor/GetAll', { method: 'get' })
    let result1 = await result.json()
    return result1
}

async function addActor(name:string){
    let result = await fetch (`http://localhost:5000/api/actor/`, { method: 'put', body:JSON.stringify({name}), credentials:'same-origin', headers: {'content-type':'application/json'} })
}

export class ActorsManager extends React.Component<RouteComponentProps<{}>, { actors: Models.Actor[] | "loading", actorToAdd: { name: string }, addingActor: boolean }>{
    constructor(props: RouteComponentProps<{}>) {
        super(props)
        this.state = { actors: "loading", actorToAdd: { name: "" }, addingActor: false }
    }

    componentWillMount() {
        loadActors().then(a => this.setState({ ... this.state, actors: a }))
    }

    public render() {
        if (this.state.actors == "loading") return <div>Loading..</div>
        return <div className="actors">
            {this.state.actors.map(a =>
                <div className="actors-actor">
                    <Actor actor={a} preview={true} />
                    <button onClick={() => 
                    <Link to={"/actors/false/" + a.id}>
                    </Link>
                    }>Expand actor</button>
                </div>
            )}
            <div>
                <input onChange={data => { this.setState({ ...this.state, actorToAdd: { ...this.state.actorToAdd, name: data.target.value} }) }} />
                <button onClick={() => {
                    if (this.state.actorToAdd.name == "") return;
                    this.setState({ ...this.state, addingActor: true })
                    addActor(this.state.actorToAdd.name).then(() => {
                        this.setState({ ...this.state, addingActor: false })
                    })
                }}> Add </button>
                <textarea value={this.state.addingActor ? "Waiting for response..." : ""} disabled={true} />
            </div>
        </div>
    }
}