import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom'
import { Layout } from './components/Layout'
import { GamesPage } from './pages/GamesPage'
import { PublishersPage } from './pages/PublishersPage'
import './App.css'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route index element={<Navigate to="/games" replace />} />
          <Route path="/games" element={<GamesPage />} />
          <Route path="/publishers" element={<PublishersPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
