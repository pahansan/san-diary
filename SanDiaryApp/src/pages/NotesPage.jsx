import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/client';
import { Header } from '../components/Header';
import './NotesPage.css';

export const NotesPage = () => {
    const [notes, setNotes] = useState([]);
    const [search, setSearch] = useState('');
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

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

    return (
        <div className="notes-page">
            <Header />
            <div className="container">
                <h1>Мои Заметки</h1>
                <button onClick={createNewNote} className="btn-primary">+ Новая заметка</button>

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
                                <h3>{note.title}</h3>
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
