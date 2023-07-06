import { useNavigate } from 'react-router-dom';

const ViewBook = ({ book }) => {
  const navigate = useNavigate();

  const handleViewBook = () => {
    navigate(`/book?${new URLSearchParams(book).toString()}`);
  };

  return (
    <button onClick={handleViewBook}>View Book</button>
  );
};

export default ViewBook;
