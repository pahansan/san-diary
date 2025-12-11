import { createContext, useContext, useState, useEffect } from 'react';
import api from '../api/client';

const AuthContext = createContext(null);

const CLAIMS = {
    EMAIL: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    ROLE: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    USER_ID: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
};

const decodeToken = (token) => {
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return {
            email: payload[CLAIMS.EMAIL],
            role: payload[CLAIMS.ROLE],
            userId: parseInt(payload[CLAIMS.USER_ID], 10)
        };
    } catch (error) {
        console.error('Failed to decode token:', error);
        return null;
    }
};

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            const decoded = decodeToken(token);
            if (decoded) {
                setUser(decoded);
            } else {
                localStorage.removeItem('token');
            }
        }
        setLoading(false);
    }, []);

    const login = async (email, password) => {
        const response = await api.post('/auth/login', { email, password });
        const { token } = response.data;

        const decoded = decodeToken(token);
        if (decoded) {
            localStorage.setItem('token', token);
            setUser(decoded);
        }

        return response.data;
    };

    const register = async (email, password) => {
        const response = await api.post('/auth/register', { email, password });
        return response.data;
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, register, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
