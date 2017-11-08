import * as React from 'react'
import { RouteComponentProps } from 'react-router'
import { Route, NavLink, Link } from 'react-router-dom'
import * as Models from "../Models"
import { Actor } from "./Actor"

async function loadMovie(id: number) {
    let res = await fetch(`http://localhost:5000/api/movie/GetMovie/${id}`, { method: 'get', credentials: 'same-origin', headers: { 'content-type': 'application/json' } })
    if (res.status == 404) return;
    let res1 = await res.json()
    return res1
}

export class MovieRoute extends React.Component<RouteComponentProps<{ preview: string, movie: number }>, { movie: Models.Movie | "loading" }>{
    constructor(props: RouteComponentProps<{ preview: string, movie: number }>) {
        super(props)
        this.state = { movie: "loading" }
    }

    componentWillMount() {
        loadMovie(this.props.match.params.movie).then(m => this.setState({ ... this.state, movie: m }))
    }

    public render() {
        if (this.state.movie == "loading") return <div>Loading...</div>
        return <Movie preview={this.props.match.params.preview == 'true'} movie={this.state.movie} />
    }
}

export class Movie extends React.Component<{ preview: boolean, movie: Models.Movie }, {}>{
    constructor(props: { preview: boolean, movie: Models.Movie }) {
        super(props)
        this.state = {}
    }

    public render() {
        if (this.props.preview) {
            return <div>{this.props.movie.title}</div>
        }
        else {
            return <div>
                <div> Movie title: {this.props.movie.title} </div>
                <div> Movie release: {this.props.movie.release} </div>
                <div className="movie-actors">Actors:
                        {this.props.movie.actors.map(a => <div className="movie-actors-actor">
                        <Actor actor={a} preview={true} />
                        <Link to={"/actors/false/" + a.id}>
                            <button>Expand actor</button>
                        </Link>
                    </div>)}
                </div>
            </div>
        }
    }
}