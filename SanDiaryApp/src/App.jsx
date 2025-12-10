import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import { AuthPage } from './pages/AuthPage';
import { NotesPage } from './pages/NotesPage';
import { NoteDetailPage } from './pages/NoteDetailPage';
import { AdminDashboard } from './pages/AdminDashboard';
import { UsersPage } from './pages/UsersPage';

const PrivateRoute = ({ children, requireAdmin = false }) => {
  const { user, loading } = useAuth();

  if (loading) return <div>Загрузка...</div>;

  if (!user) return <Navigate to="/auth" />;

  if (requireAdmin && user.role !== 'Admin') {
    return <Navigate to="/notes" />;
  }

  return children;
};

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/auth" element={<AuthPage />} />
        <Route path="/" element={<Navigate to="/auth" />} />

        <Route path="/notes" element={
          <PrivateRoute>
            <NotesPage />
          </PrivateRoute>
        } />

        <Route path="/notes/:id" element={
          <PrivateRoute>
            <NoteDetailPage />
          </PrivateRoute>
        } />

        <Route path="/admin" element={
          <PrivateRoute requireAdmin={true}>
            <AdminDashboard />
          </PrivateRoute>
        } />

        <Route path="/users" element={
          <PrivateRoute requireAdmin={true}>
            <UsersPage />
          </PrivateRoute>
        } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
