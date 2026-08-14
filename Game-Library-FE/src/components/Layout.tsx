import { NavLink, Outlet } from 'react-router-dom'
import { ThemeToggle } from './ThemeToggle'

export function Layout() {
  return (
    <>
      <header className="site-header">
        <div className="site-header-top">
          <h1>Game Library</h1>
          <ThemeToggle />
        </div>
        <nav className="site-nav">
          <NavLink to="/games" className={({ isActive }) => (isActive ? 'active' : '')}>
            Games
          </NavLink>
          <NavLink to="/publishers" className={({ isActive }) => (isActive ? 'active' : '')}>
            Publishers
          </NavLink>
        </nav>
      </header>
      <main>
        <Outlet />
      </main>
    </>
  )
}
