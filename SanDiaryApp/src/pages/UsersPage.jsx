import { useState, useEffect } from 'react';
import { Header } from '../components/Header';
import api from '../api/client';
import './UsersPage.css';

export const UsersPage = () => {
    const [users, setUsers] = useState([]);
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

    return (
        <div className="users-page">
            <Header />
            <div className="container">
                <h1>Управление пользователями</h1>
                {loading ? (
                    <div>Загрузка...</div>
                ) : (
                    <div className="users-list">
                        {users.map(user => (
                            <div key={user.id} className="user-card">
                                <div>
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
