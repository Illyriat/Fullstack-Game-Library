import { useEffect, useState } from 'react'
import { getGames } from '../api/client'
import type { GameSummary, GetGamesParams, PaginatedResult } from '../api/types'

interface UseGamesState {
  data: PaginatedResult<GameSummary> | null
  loading: boolean
  error: string | null
}

export function useGames(params: GetGamesParams): UseGamesState {
  const [data, setData] = useState<PaginatedResult<GameSummary> | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    getGames(params)
      .then((result) => {
        if (cancelled) return
        setData(result)
        setError(null)
      })
      .catch((err: unknown) => {
        if (cancelled) return
        setError(err instanceof Error ? err.message : 'Failed to load games')
      })

    return () => {
      cancelled = true
    }
  }, [params])

  return { data, error, loading: data === null && error === null }
}
