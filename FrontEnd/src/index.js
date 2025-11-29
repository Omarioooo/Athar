import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { ProvideContext } from './Auth/Auth';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <ProvideContext>
    <App />
  </ProvideContext>
);