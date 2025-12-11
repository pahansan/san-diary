import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Header } from '../components/Header';
import api from '../api/client';
import './UsersPage.css';

export const UsersPage = () => {
    const navigate = useNavigate();
    const [users, setUsers] = useState([]);
    const [search, setSearch] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadUsers();
    }, []);

    const loadUsers = async () => {
        try {
            const response = await api.get('/user');
            setUsers(response.data);
        } catch (err) {
            console.error('Failed to load users');
        } finally {
            setLoading(false);
        }
    };

    const deleteUser = async (userId) => {
        if (window.confirm('Удалить пользователя и все его заметки?')) {
            try {
                await api.delete(`/user/${userId}`);
                loadUsers();
            } catch (err) {
                console.error('Failed to delete user');
            }
        }
    };

    const filteredUsers = users.filter(user =>
        user.email.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="users-page">
            <Header />
            <div className="container">
                <div className="page-header">
                    <button onClick={() => navigate('/admin')} className="btn-back">
                        ← Назад в панель администратора
                    </button>
                    <h1>Управление пользователями</h1>
                </div>

                <input
                    type="text"
                    placeholder="Поиск по email..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="search-input"
                />

                {loading ? (
                    <div>Загрузка...</div>
                ) : filteredUsers.length === 0 ? (
                    <div className="no-results">Пользователи не найдены</div>
                ) : (
                    <div className="users-list">
                        {filteredUsers.map(user => (
                            <div key={user.id} className="user-card">
                                <div className="user-info">
                                    <strong>{user.email}</strong>
                                    <span className="role-badge">{user.role}</span>
                                </div>
                                <button onClick={() => deleteUser(user.id)} className="btn-danger">
                                    Удалить
                                </button>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};
