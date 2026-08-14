export interface PaginatedResult<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export interface GameSummary {
  id: number
  name: string
  releaseYear: number
  genreId: number
  genreName: string
  publisherId: number | null
  publisherName: string | null
}

export interface PublisherSummary {
  id: number
  name: string
}

export interface GenreSummary {
  id: number
  name: string
}

export interface GetGamesParams {
  name?: string
  releaseYear?: number
  genreId?: number
  publisherId?: number
  page: number
}

export interface GetPublishersParams {
  name?: string
  page: number
}

export interface GetGenresParams {
  name?: string
  page: number
}
