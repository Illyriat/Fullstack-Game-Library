import { useMemo, useState } from 'react'
import { Pagination } from '../components/Pagination'
import { useDebouncedValue } from '../hooks/useDebouncedValue'
import { useGames } from '../hooks/useGames'
import { useGenres } from '../hooks/useGenres'
import { usePublishers } from '../hooks/usePublishers'
import type { GetGenresParams, GetPublishersParams } from '../api/types'

const PUBLISHER_DROPDOWN_PARAMS: GetPublishersParams = { page: 1 }
const GENRE_DROPDOWN_PARAMS: GetGenresParams = { page: 1 }

export function GamesPage() {
  const [name, setName] = useState('')
  const [releaseYear, setReleaseYear] = useState('')
  const [genreId, setGenreId] = useState('')
  const [publisherId, setPublisherId] = useState('')
  const [page, setPage] = useState(1)

  const debouncedName = useDebouncedValue(name, 300)

  const { data: publishersData } = usePublishers(PUBLISHER_DROPDOWN_PARAMS)
  const { data: genresData } = useGenres(GENRE_DROPDOWN_PARAMS)

  const gamesParams = useMemo(
    () => ({
      name: debouncedName || undefined,
      releaseYear: releaseYear ? Number(releaseYear) : undefined,
      genreId: genreId ? Number(genreId) : undefined,
      publisherId: publisherId ? Number(publisherId) : undefined,
      page,
    }),
    [debouncedName, releaseYear, genreId, publisherId, page],
  )

  const { data, loading, error } = useGames(gamesParams)

  function resetToFirstPage<T>(setter: (value: T) => void) {
    return (value: T) => {
      setter(value)
      setPage(1)
    }
  }

  return (
    <section>
      <h2>Games</h2>

      <form className="filters" onSubmit={(e) => e.preventDefault()}>
        <label>
          Name
          <input
            type="text"
            value={name}
            onChange={(e) => resetToFirstPage(setName)(e.target.value)}
            placeholder="Search by name"
          />
        </label>

        <label>
          Release year
          <input
            type="number"
            value={releaseYear}
            onChange={(e) => resetToFirstPage(setReleaseYear)(e.target.value)}
            placeholder="e.g. 2015"
          />
        </label>

        <label>
          Genre
          <select value={genreId} onChange={(e) => resetToFirstPage(setGenreId)(e.target.value)}>
            <option value="">All genres</option>
            {genresData?.items.map((g) => (
              <option key={g.id} value={g.id}>
                {g.name}
              </option>
            ))}
          </select>
        </label>

        <label>
          Publisher
          <select value={publisherId} onChange={(e) => resetToFirstPage(setPublisherId)(e.target.value)}>
            <option value="">All publishers</option>
            {publishersData?.items.map((p) => (
              <option key={p.id} value={p.id}>
                {p.name}
              </option>
            ))}
          </select>
        </label>
      </form>

      {error && <p className="error">{error}</p>}
      {loading && <p>Loading games...</p>}

      {!loading && !error && data && (
        <>
          <p className="result-count">{data.totalCount} game{data.totalCount === 1 ? '' : 's'} found</p>

          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Release year</th>
                <th>Genre</th>
                <th>Publisher</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((game) => (
                <tr key={game.id}>
                  <td>{game.name}</td>
                  <td>{game.releaseYear}</td>
                  <td>{game.genreName}</td>
                  <td>{game.publisherName ?? '—'}</td>
                </tr>
              ))}
              {data.items.length === 0 && (
                <tr>
                  <td colSpan={4} className="empty-row">
                    No games match these filters.
                  </td>
                </tr>
              )}
            </tbody>
          </table>

          <Pagination page={data.page} totalPages={data.totalPages} onPageChange={setPage} />
        </>
      )}
    </section>
  )
}
