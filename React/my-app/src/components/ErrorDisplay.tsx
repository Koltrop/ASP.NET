import React from 'react';

interface ErrorDisplayProps {
  error: string;
}

const ErrorDisplay: React.FC<ErrorDisplayProps> = ({ error }) => {
  return (
    <div style={{ background: 'lightcoral', padding: '10px', marginTop: '10px' }}>
      Error: {error}
    </div>
  );
};

export default ErrorDisplay;
