import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { useNavigate } from 'react-router-dom';
import { FaSun, FaMoon } from 'react-icons/fa';
import './AuthPage.css';

export const AuthPage = () => {
    const [isLogin, setIsLogin] = useState(true);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login, register, user } = useAuth();
    const { theme, toggleTheme } = useTheme();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            if (isLogin) {
                const data = await login(email, password);
                if (data.role === 'Admin') {
                    navigate('/admin');
                } else {
                    navigate('/notes');
                }
            } else {
                await register(email, password);
                setIsLogin(true);
            }
        } catch (err) {
            setError(err.response?.data?.errors?.[0] || 'Ошибка');
        }
    };

    if (user) {
        navigate(user.role === 'Admin' ? '/admin' : '/notes');
        return null;
    }

    return (
        <div className="auth-container">
            <div className="auth-form">
                <div className="auth-header">
                    <h2>{isLogin ? 'Вход' : 'Регистрация'}</h2>
                    <button onClick={toggleTheme} className="theme-toggle-btn" title="Переключить тему">
                        {theme === 'light' ? <FaMoon /> : <FaSun />}
                    </button>
                </div>

                <form onSubmit={handleSubmit}>
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <input
                        type="password"
                        placeholder="Пароль"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    {error && <div className="error">{error}</div>}
                    <button type="submit">{isLogin ? 'Войти' : 'Зарегистрироваться'}</button>
                </form>

                <button className="toggle-btn" onClick={() => setIsLogin(!isLogin)}>
                    {isLogin ? 'Нет аккаунта? Зарегистрироваться' : 'Уже есть аккаунт? Войти'}
                </button>
            </div>
        </div>
    );
};
