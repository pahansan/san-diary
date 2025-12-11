import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { useNavigate } from 'react-router-dom';
import { FaSun, FaMoon } from 'react-icons/fa';
import './Header.css';

export const Header = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/auth');
    };

    if (!user) return null;

    return (
        <header className="header">
            <div className="header-content">
                <span className="user-email">{user?.email || '—'}</span> { }
                <button onClick={handleLogout} className="logout-btn">Выйти</button>
            </div>
        </header>
    );
};
