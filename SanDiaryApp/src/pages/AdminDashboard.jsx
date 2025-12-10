import { useNavigate } from 'react-router-dom';
import { Header } from '../components/Header';
import './AdminDashboard.css';

export const AdminDashboard = () => {
    const navigate = useNavigate();

    return (
        <div className="admin-dashboard">
            <Header />
            <div className="container">
                <h1>Админ-панель</h1>
                <div className="admin-menu">
                    <button onClick={() => navigate('/notes')} className="admin-btn">
                        <h3>Мои заметки</h3>
                        <p>Управлять своими записями дневника</p>
                    </button>
                    <button onClick={() => navigate('/users')} className="admin-btn">
                        <h3>Пользователи</h3>
                        <p>Управлять пользователями системы</p>
                    </button>
                </div>
            </div>
        </div>
    );
};
