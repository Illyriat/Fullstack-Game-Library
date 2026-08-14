import { useEffect, useState } from 'react'
import { getGenres } from '../api/client'
import type { GenreSummary, GetGenresParams, PaginatedResult } from '../api/types'

interface UseGenresState {
  data: PaginatedResult<GenreSummary> | null
  loading: boolean
  error: string | null
}

export function useGenres(params: GetGenresParams): UseGenresState {
  const [data, setData] = useState<PaginatedResult<GenreSummary> | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    getGenres(params)
      .then((result) => {
        if (cancelled) return
        setData(result)
        setError(null)
      })
      .catch((err: unknown) => {
        if (cancelled) return
        setError(err instanceof Error ? err.message : 'Failed to load genres')
      })

    return () => {
      cancelled = true
    }
  }, [params])

  return { data, error, loading: data === null && error === null }
}
