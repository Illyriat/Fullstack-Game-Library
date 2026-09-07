import { renderHook, act } from '@testing-library/react'
import { useDebouncedValue } from '../../src/hooks/useDebouncedValue'

describe('useDebouncedValue', () => {
  beforeEach(() => {
    vi.useFakeTimers()
  })

  afterEach(() => {
    vi.useRealTimers()
  })

  it('returns the initial value immediately', () => {
    const { result } = renderHook(() => useDebouncedValue('initial', 200))
    expect(result.current).toBe('initial')
  })

  it('only updates after the delay has elapsed', () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebouncedValue(value, delay),
      { initialProps: { value: 'initial', delay: 200 } },
    )

    rerender({ value: 'updated', delay: 200 })
    expect(result.current).toBe('initial')

    act(() => {
      vi.advanceTimersByTime(199)
    })
    expect(result.current).toBe('initial')

    act(() => {
      vi.advanceTimersByTime(1)
    })
    expect(result.current).toBe('updated')
  })
})
