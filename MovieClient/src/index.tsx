import * as React from 'react'
import * as ReactDOM from 'react-dom'

import {ActorRoute} from './components/Actor'
import {MovieRoute} from './components/Movie'
import {ActorsManager } from './components/ActorsManager'
import {MoviesManager } from './components/MoviesManager'
import Routing from './components/Routing'

import {
  BrowserRouter as Router,
  Route, 
  RouteComponentProps,
  Link
} from 'react-router-dom'

export default class Home extends React.Component<RouteComponentProps<{}>, {hometext:string}>{
  constructor(props:RouteComponentProps<{}>){
    super(props)
    this.state = {hometext:"loading"}    
  }

  render() {
    return <div>
      <h2>Home</h2>
    </div>
  }
}


ReactDOM.render(
  <Routing />,
  document.getElementById("root")
);