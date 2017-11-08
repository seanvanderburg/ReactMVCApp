import * as React from 'react'
import * as ReactDOM from 'react-dom'

import {ActorRoute} from '../components/Actor'
import {MovieRoute} from '../components/Movie'
import {ActorsManager } from '../components/ActorsManager'
import {MoviesManager } from '../components/MoviesManager'
import Home from '../index'

import {
  BrowserRouter as Router,
  Route, 
  RouteComponentProps,
  Link
} from 'react-router-dom'

export default class Routing extends React.Component<{},{}> {
    render() {
      return (
        <Router>
          <div className="routing">
            <ul>
              <li><Link to="/">Home</Link></li>
              <li><Link to="/movies">Movies</Link></li>
              <li><Link to="/actors">Actors</Link></li>
            </ul>
            <hr />
            <Route exact path="/" component={Home} />
            <Route exact path="/movies" component={MoviesManager} />
            <Route exact path="/actors" component={ActorsManager} /> 
            <Route exact path="/actors/:preview/:actor" component={ActorRoute} /> 
            <Route exact path="/movies/:preview/:movie" component={MovieRoute} /> 
          </div>
        </Router>
      );
    }
  }