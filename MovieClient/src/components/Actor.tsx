import * as React from 'react'
import { RouteComponentProps } from 'react-router'
import * as Models from "../Models"

async function loadActor(id:number):Promise<Models.Actor> {
    let result = await fetch(`http://localhost:5000/api/actor/GetActor/${id}`, { method: 'get', credentials:'same-origin', headers: {'content-type':'application/json'}})
    let result1 = await result.json()
    return result1    
}

export class ActorRoute extends React.Component<RouteComponentProps<{preview:string, actor:number}>, {actor:Models.Actor | "loading"}>{
    constructor(props:RouteComponentProps<{preview:string, actor:number}>){
        super(props)
        this.state = {actor:"loading"}
    }

    componentWillMount(){
        loadActor(this.props.match.params.actor).then(a => this.setState({... this.state, actor : a}))
    }    

    public render(){
        if(this.state.actor == "loading") return <div>Loading...</div>
        return <Actor preview={this.props.match.params.preview == 'true'} actor={this.state.actor}/>
    }
}

export class Actor extends React.Component<{preview:boolean, actor:Models.Actor}, {}>{
    constructor(props:{preview:boolean, actor:Models.Actor}){
        super(props)
        this.state = {}
    }

    public render(){
        if(this.props.preview) 
        {
            return <div>{this.props.actor.name}</div>
        }
        else
        {
            return <div>
                <div>Actor name: {this.props.actor.name}</div>
                <div>Gender: {this.props.actor.gender}</div>
                <div>Date of Birth: {this.props.actor.birth}</div>
           </div>
        }
    }
}