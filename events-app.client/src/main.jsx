import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './css/reset.css';
import './css/grid.css';
import './css/layout.css';
import './css/custom.css';
import AuthState from './context/AuthState.jsx';

ReactDOM.createRoot(document.getElementById('root')).render(
    <AuthState>
        <App /> 
    </AuthState>
)
