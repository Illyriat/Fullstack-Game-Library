import { useEffect, useState } from 'react'

export type ThemePreference = 'system' | 'light' | 'dark'

const STORAGE_KEY = 'theme'

function readStoredTheme(): ThemePreference {
  const stored = localStorage.getItem(STORAGE_KEY)
  return stored === 'light' || stored === 'dark' ? stored : 'system'
}

export function useTheme() {
  const [theme, setTheme] = useState<ThemePreference>(readStoredTheme)

  useEffect(() => {
    if (theme === 'system') {
      document.documentElement.removeAttribute('data-theme')
      localStorage.removeItem(STORAGE_KEY)
    } else {
      document.documentElement.setAttribute('data-theme', theme)
      localStorage.setItem(STORAGE_KEY, theme)
    }
  }, [theme])

  return [theme, setTheme] as const
}
