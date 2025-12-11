import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/client';
import { Header } from '../components/Header';
import { useAuth } from '../context/AuthContext';
import './NotesPage.css';

const moodLabels = {
    VeryBad: 'Очень плохо',
    Bad: 'Плохо',
    Neutral: 'Нейтрально',
    Good: 'Хорошо',
    VeryGood: 'Отлично'
};

export const NotesPage = () => {
    const [notes, setNotes] = useState([]);
    const [search, setSearch] = useState('');
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const { user } = useAuth();

    useEffect(() => {
        loadNotes();
    }, []);

    const loadNotes = async () => {
        try {
            const response = await api.get('/note');
            setNotes(response.data);
        } catch (err) {
            console.error('Failed to load notes');
        } finally {
            setLoading(false);
        }
    };

    const filteredNotes = notes.filter(note =>
        note.title.toLowerCase().includes(search.toLowerCase())
    );

    const createNewNote = () => {
        navigate('/notes/new');
    };

    const openNote = (id) => {
        navigate(`/notes/${id}`);
    };

    const getMoodColor = (mood) => {
        const colors = {
            VeryBad: '#e74c3c',
            Bad: '#e67e22',
            Neutral: '#95a5a6',
            Good: '#2ecc71',
            VeryGood: '#27ae60'
        };
        return colors[mood] || '#95a5a6';
    };

    return (
        <div className="notes-page">
            <Header />
            <div className="container">
                <div className="page-header">
                    {user?.role === 'Admin' && (
                        <button onClick={() => navigate('/admin')} className="btn-back">
                            ← Назад в панель администратора
                        </button>
                    )}
                    <h1>Мои Заметки</h1>
                    <button onClick={createNewNote} className="btn-primary">+ Новая заметка</button>
                </div>

                <input
                    type="text"
                    placeholder="Поиск по названию..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="search-input"
                />

                {loading ? (
                    <div>Загрузка...</div>
                ) : (
                    <div className="notes-grid">
                        {filteredNotes.map(note => (
                            <div key={note.id} className="note-card" onClick={() => openNote(note.id)}>
                                <div className="note-header">
                                    <h3>{note.title}</h3>
                                    <span
                                        className="mood-indicator"
                                        style={{ backgroundColor: getMoodColor(note.mood) }}
                                        title={moodLabels[note.mood]}
                                    >
                                        {moodLabels[note.mood]}
                                    </span>
                                </div>
                                <p className="note-date">
                                    Создано: {new Date(note.createdAt).toLocaleDateString()}
                                </p>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};
