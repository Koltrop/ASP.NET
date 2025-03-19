import React, { useState } from 'react';
import FactDisplay from './components/FactDisplay';
import ErrorDisplay from './components/ErrorDisplay';

const App: React.FC = () => {
  const [fact, setFact] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleFetch = async () => {
    try {
      const response = await fetch('https://catfact.ninja/fact');
      if (!response.ok) {
        throw new Error(`Error ${response.status}: ${response.statusText}`);
      }
      const data = await response.json();
      setFact(data.fact);
      setError(null);
    } catch (err: any) {
      setError(err.message);
      setFact(null);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <button onClick={handleFetch}>Get a fun fact about cats</button>
      {/* If the fact was obtained, the component is shown with a green background */}
      {fact && <FactDisplay fact={fact} />}
      {/* If an error occurs, the component is displayed with a red background. */}
      {error && <ErrorDisplay error={error} />}
    </div>
  );
};

export default App;
