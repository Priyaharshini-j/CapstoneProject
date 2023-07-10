import React, { useState } from 'react';

const CreateCritiqueComponent = ({ ISBN, handleOpen, handleClose }) => {
  const [formData, setFormData] = useState({
    ISBN:ISBN,
    userId: sessionStorage.getItem("userId"),
    critiqueDesc: '',
  });
  const [errors, setErrors] = useState({});

  const handleSubmit = (event) => {
    event.preventDefault();

    // Perform form validation
    const formErrors = {};
    if (!formData.critiqueDesc) {
      formErrors.critiqueDesc = 'Critique is required';
    }

    if (Object.keys(formErrors).length > 0) {
      // If there are errors, update the state and prevent form submission
      setErrors(formErrors);
      return;
    }

    // Clear any existing errors
    setErrors({});

    handleClose();
  };

  const handleForm = (event) => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value,
    });
  };

  return (
    <div>
      {/* Form content */}
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="critiqueDesc">Critique:</label>
          <input
            type="textarea"
            id="name"
            name="critiqueDesc"
            value={formData.critiqueDesc}
            onChange={handleForm}
          />
          {errors.critiqueDesc && <p className="error">{errors.critiqueDesc}</p>}
        </div>

        {/* ...more form fields... */}
        <button type="submit">Submit</button>
      </form>
    </div>
  );
};

export default CreateCritiqueComponent;
