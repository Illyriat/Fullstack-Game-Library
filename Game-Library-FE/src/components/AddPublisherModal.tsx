import { useEffect, useState, type FormEvent } from 'react'
import { createPublisher } from '../api/client'
import type { PublisherSummary } from '../api/types'

const MAX_NAME_LENGTH = 200

interface AddPublisherModalProps {
  onClose: () => void
  onCreated: (publisher: PublisherSummary) => void
}

export function AddPublisherModal({ onClose, onCreated }: AddPublisherModalProps) {
  const [name, setName] = useState('')
  const [fieldError, setFieldError] = useState<string | null>(null)
  const [apiError, setApiError] = useState<string | null>(null)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    function handleKeyDown(e: KeyboardEvent) {
      if (e.key === 'Escape' && !submitting) onClose()
    }

    document.addEventListener('keydown', handleKeyDown)
    return () => document.removeEventListener('keydown', handleKeyDown)
  }, [onClose, submitting])

  function validate(trimmedName: string): string | null {
    if (!trimmedName) return 'Publisher name is required.'
    if (trimmedName.length > MAX_NAME_LENGTH) {
      return `Publisher name must not exceed ${MAX_NAME_LENGTH} characters.`
    }
    return null
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()

    if (submitting) return

    const trimmedName = name.trim()
    const validationError = validate(trimmedName)

    if (validationError) {
      setFieldError(validationError)
      return
    }

    setFieldError(null)
    setApiError(null)
    setSubmitting(true)

    try {
      const created = await createPublisher({ name: trimmedName })
      onCreated(created)
    } catch (err) {
      setApiError(err instanceof Error ? err.message : 'Failed to create publisher')
    } finally {
      setSubmitting(false)
    }
  }

  function handleOverlayClick() {
    if (!submitting) onClose()
  }

  return (
    <div className="modal-overlay" onClick={handleOverlayClick}>
      <div
        className="modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby="add-publisher-title"
        onClick={(e) => e.stopPropagation()}
      >
        <h2 id="add-publisher-title">Add Publisher</h2>

        <form onSubmit={handleSubmit}>
          <label>
            Name
            <input
              type="text"
              value={name}
              onChange={(e) => {
                setName(e.target.value)
                if (fieldError) setFieldError(null)
              }}
              disabled={submitting}
              autoFocus
              maxLength={MAX_NAME_LENGTH}
            />
          </label>

          {fieldError && <p className="error">{fieldError}</p>}
          {apiError && <p className="error">{apiError}</p>}

          <div className="modal-actions">
            <button type="button" onClick={onClose} disabled={submitting}>
              Cancel
            </button>
            <button type="submit" className="primary" disabled={submitting}>
              {submitting ? 'Creating...' : 'Create'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
