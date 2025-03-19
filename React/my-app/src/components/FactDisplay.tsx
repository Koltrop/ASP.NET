import React from 'react';

interface FactDisplayProps {
  fact: string;
}

const FactDisplay: React.FC<FactDisplayProps> = ({ fact }) => {
  return (
    <div style={{ background: 'lightgreen', padding: '10px', marginTop: '10px' }}>
      {fact}
    </div>
  );
};

export default FactDisplay;
