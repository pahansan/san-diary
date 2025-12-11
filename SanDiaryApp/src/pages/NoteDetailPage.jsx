import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import remarkBreaks from 'remark-breaks';
import api from '../api/client';
import { Header } from '../components/Header';
import './NoteDetailPage.css';

const moodLabels = {
    VeryBad: 'Очень плохо',
    Bad: 'Плохо',
    Neutral: 'Нейтрально',
    Good: 'Хорошо',
    VeryGood: 'Отлично'
};

export const NoteDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [note, setNote] = useState(null);
    const [isEditing, setIsEditing] = useState(id === 'new');
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [mood, setMood] = useState('Neutral');
    const [loading, setLoading] = useState(true);
    const [previewMode, setPreviewMode] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (id !== 'new') {
            loadNote();
        } else {
            setLoading(false);
            setIsEditing(true);
        }
    }, [id]);

    const loadNote = async () => {
        try {
            const response = await api.get(`/note/${id}`);
            setNote(response.data);
            setTitle(response.data.title);
            setContent(response.data.content);
            setMood(response.data.mood);
        } catch (err) {
            console.error('Failed to load note');
        } finally {
            setLoading(false);
        }
    };

    const saveNote = async () => {
        if (!title || title.trim() === '') {
            setError('Заголовок заметки обязателен');
            return;
        }

        setError('');
        try {
            const data = { title: title.trim(), content, mood };
            if (id === 'new') {
                await api.post('/note', data);
            } else {
                await api.put(`/note/${id}`, data);
            }
            navigate('/notes');
        } catch (err) {
            console.error('Failed to save note');
            setError(err.response?.data?.errors?.[0] || 'Не удалось сохранить заметку');
        }
    };

    const deleteNote = async () => {
        if (window.confirm('Удалить заметку?')) {
            try {
                await api.delete(`/note/${id}`);
                navigate('/notes');
            } catch (err) {
                console.error('Failed to delete note');
            }
        }
    };

    if (loading) return <div>Загрузка...</div>;

    return (
        <div className="note-detail-page">
            <Header />
            <div className="container">
                <div className="page-header">
                    <button onClick={() => navigate('/notes')} className="btn-back">
                        ← Назад к заметкам
                    </button>
                    {!isEditing && id !== 'new' && (
                        <button onClick={() => setIsEditing(true)} className="btn-secondary">
                            Редактировать
                        </button>
                    )}
                    {isEditing && id !== 'new' && (
                        <button onClick={() => setIsEditing(false)} className="btn-secondary">
                            Отмена
                        </button>
                    )}
                    {!isEditing && id !== 'new' && (
                        <button onClick={deleteNote} className="btn-danger">Удалить</button>
                    )}
                </div>

                {isEditing ? (
                    <div className="edit-mode">
                        {error && <div className="error-message">{error}</div>}

                        <input
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            className="title-input"
                            placeholder="Заголовок *"
                            required
                        />

                        <div className="mood-preview-wrapper">
                            <select value={mood} onChange={(e) => setMood(e.target.value)} className="mood-select">
                                <option value="VeryBad">Очень плохо</option>
                                <option value="Bad">Плохо</option>
                                <option value="Neutral">Нейтрально</option>
                                <option value="Good">Хорошо</option>
                                <option value="VeryGood">Отлично</option>
                            </select>

                            <label className="preview-toggle">
                                <input
                                    type="checkbox"
                                    checked={previewMode}
                                    onChange={(e) => setPreviewMode(e.target.checked)}
                                />
                                Markdown
                            </label>
                        </div>

                        <div className="editor-container">
                            {previewMode ? (
                                <div className="markdown-preview">
                                    <ReactMarkdown remarkPlugins={[remarkGfm, remarkBreaks]}>
                                        {content}
                                    </ReactMarkdown>
                                </div>
                            ) : (
                                <textarea
                                    value={content}
                                    onChange={(e) => setContent(e.target.value)}
                                    className="content-textarea"
                                    placeholder="Поддерживается Markdown..."
                                />
                            )}
                        </div>

                        <button onClick={saveNote} className="btn-primary">Сохранить</button>
                    </div>
                ) : (
                    <div className="view-mode">
                        <h1>{note.title}</h1>
                        <div className="note-meta">
                            <span>Настроение: {moodLabels[note.mood]}</span>
                            <span>Создано: {new Date(note.createdAt).toLocaleString()}</span>
                            <span>Изменено: {new Date(note.updatedAt).toLocaleString()}</span>
                        </div>
                        <div className="note-content">
                            <ReactMarkdown remarkPlugins={[remarkGfm, remarkBreaks]}>
                                {note.content}
                            </ReactMarkdown>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};
