import * as React from 'react'
import * as ReactDOM from 'react-dom'
import { RouteComponentProps } from 'react-router'
import * as Models from "../Models"
import { Actor } from './Actor'

import {
    BrowserRouter as Router,
    Route,
    Link
} from 'react-router-dom'

async function loadMovies(): Promise<Models.Movie[]> {
    let result = await fetch('http://localhost:5000/api/movie/GetAll', { method: 'get' })
    let result1 = await result.json()
    return result1
}


async function searchMovie(title: string) {
    let res = await fetch(`http://localhost:5000/api/movie/GetMovieByTitle/${title}`, { method: 'get', credentials: 'same-origin', headers: { 'content-type': 'application/json' } })
    if (res.status == 404) return;    
    let result1 = await res.json()
    return result1
}

export class MoviesManager extends React.Component<RouteComponentProps<{}>, { movies: Models.Movie[] | "loading", movieToSearch: { title: string }, searchingMovie: boolean, foundMovie:Models.Movie | null }>{
    constructor(props: RouteComponentProps<{}>) {
        super(props)
        this.state = { movies: "loading", movieToSearch: { title: "" }, searchingMovie: false, foundMovie:null}
    }

    componentWillMount() {
        loadMovies().then(ms => this.setState({ ... this.state, movies: ms }))
    }

    public render() {
        if (this.state.movies == "loading") return <div>Loading..</div>
        return <div className="movies">
            {this.state.movies.map(m =>
                <div className="movies-movie">
                    <div>Movie: {m.title}</div>
                    <Link to={"/movies/false/" + m.id}>
                        <button>Select {m.title}</button>
                    </Link>
                    <div className="movies-movie-actors">
                        <div>Actors:</div>
                        {m.actors.map(a =>
                            <div className="movies movie actors actor">
                                <Link to={"/actors/false/" + a.id}>
                                    <button><form action=""></form> {a.name} Data</button>
                                </Link>
                            </div>)}
                    </div>
                </div>)}
            <div>
            <hr />
                <div>Search movie:</div>
                <input onChange={(event) => { this.setState({ ...this.state, movieToSearch: { ...this.state.movieToSearch, title: event.target.value } }) }} />
                <button onClick={() => {
                    if (this.state.movieToSearch.title == "") return;
                    this.setState({ ...this.state, searchingMovie: true })
                    searchMovie(this.state.movieToSearch.title).then(movie => {
                        this.setState({ ...this.state, searchingMovie: false, foundMovie: movie})
                    })
                }}> Search </button>
                <div>
                    <div>{this.state.searchingMovie ? "Waiting for response..." : (this.state.foundMovie? ("Found movie: " + this.state.foundMovie.title + " with release date: "+ this.state.foundMovie.release + " and actors: " + this.state.foundMovie.actors.map(a => a.name)) : "")} </div>
                </div>
            </div>
        </div>
    }
}