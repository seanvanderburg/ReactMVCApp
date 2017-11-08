export type Movie = {
    id:number,
    title:string,
    release:Date,
    actors:Actor[]
}

export type Actor = {
    id:number,
    name:string,
    birth:Date,
    gender:string
}