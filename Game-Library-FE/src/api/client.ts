import type {
  GameSummary,
  GenreSummary,
  GetGamesParams,
  GetGenresParams,
  GetPublishersParams,
  PaginatedResult,
  PublisherSummary,
} from './types'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

function buildQuery(params: Record<string, string | number | undefined>): string {
  const search = new URLSearchParams()

  for (const [key, value] of Object.entries(params)) {
    if (value !== undefined && value !== '') {
      search.set(key, String(value))
    }
  }

  const query = search.toString()
  return query ? `?${query}` : ''
}

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`)

  if (!response.ok) {
    throw new Error(`Request to ${path} failed with status ${response.status}`)
  }

  return (await response.json()) as T
}

export function getGames(params: GetGamesParams): Promise<PaginatedResult<GameSummary>> {
  const query = buildQuery({
    name: params.name,
    releaseYear: params.releaseYear,
    genreId: params.genreId,
    publisherId: params.publisherId,
    page: params.page,
  })

  return getJson<PaginatedResult<GameSummary>>(`/api/games${query}`)
}

export function getPublishers(params: GetPublishersParams): Promise<PaginatedResult<PublisherSummary>> {
  const query = buildQuery({
    name: params.name,
    page: params.page,
  })

  return getJson<PaginatedResult<PublisherSummary>>(`/api/publishers${query}`)
}

export function getGenres(params: GetGenresParams): Promise<PaginatedResult<GenreSummary>> {
  const query = buildQuery({
    name: params.name,
    page: params.page,
  })

  return getJson<PaginatedResult<GenreSummary>>(`/api/genres${query}`)
}
