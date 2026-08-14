import { useMemo, useState } from 'react'
import { Pagination } from '../components/Pagination'
import { useDebouncedValue } from '../hooks/useDebouncedValue'
import { usePublishers } from '../hooks/usePublishers'

export function PublishersPage() {
  const [name, setName] = useState('')
  const [page, setPage] = useState(1)

  const debouncedName = useDebouncedValue(name, 300)

  const publishersParams = useMemo(
    () => ({ name: debouncedName || undefined, page }),
    [debouncedName, page],
  )

  const { data, loading, error } = usePublishers(publishersParams)

  function handleNameChange(value: string) {
    setName(value)
    setPage(1)
  }

  return (
    <section>
      <h2>Publishers</h2>

      <form className="filters" onSubmit={(e) => e.preventDefault()}>
        <label>
          Name
          <input
            type="text"
            value={name}
            onChange={(e) => handleNameChange(e.target.value)}
            placeholder="Search by name"
          />
        </label>
      </form>

      {error && <p className="error">{error}</p>}
      {loading && <p>Loading publishers...</p>}

      {!loading && !error && data && (
        <>
          <p className="result-count">
            {data.totalCount} publisher{data.totalCount === 1 ? '' : 's'} found
          </p>

          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((publisher) => (
                <tr key={publisher.id}>
                  <td>{publisher.name}</td>
                </tr>
              ))}
              {data.items.length === 0 && (
                <tr>
                  <td className="empty-row">No publishers match this search.</td>
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
