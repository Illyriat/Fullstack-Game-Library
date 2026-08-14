import { useEffect, useState } from 'react'
import { getPublishers } from '../api/client'
import type { GetPublishersParams, PaginatedResult, PublisherSummary } from '../api/types'

interface UsePublishersState {
  data: PaginatedResult<PublisherSummary> | null
  loading: boolean
  error: string | null
}

export function usePublishers(params: GetPublishersParams): UsePublishersState {
  const [data, setData] = useState<PaginatedResult<PublisherSummary> | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    getPublishers(params)
      .then((result) => {
        if (cancelled) return
        setData(result)
        setError(null)
      })
      .catch((err: unknown) => {
        if (cancelled) return
        setError(err instanceof Error ? err.message : 'Failed to load publishers')
      })

    return () => {
      cancelled = true
    }
  }, [params])

  return { data, error, loading: data === null && error === null }
}
