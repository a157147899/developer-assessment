import React, { useState } from 'react';
import { Button, Col, Container, Form, Row, Spinner, Stack } from 'react-bootstrap';

interface AddTodoItemProps {
  addTodoItem: (description: string, callback: (success: boolean) => void) => void;
  showError: (error: string) => void;
}

const AddTodoItem = ({ addTodoItem, showError }: AddTodoItemProps) => {
  const [desc, setDesc] = useState('');
  const [adding, setAdding] = useState(false);

  const clearDesc = () => {
    setDesc('');
  };

  const handleAdd = () => {
    if (!desc || desc.trim().length === 0) {
      showError('Description cannot be empty.');
      return;
    }
    setAdding(true);

    addTodoItem(desc, (success) => {
      setAdding(false);
      success && clearDesc();
    });
  };

  const handleClear = () => {
    clearDesc();
  };

  const handleDescChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setDesc(event.target.value);
  };

  return (
    <Container>
      <h1>Add Item</h1>
      <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
        <Form.Label column sm="2">
          Description
        </Form.Label>
        <Col md="6">
          <Form.Control type="text" placeholder="Enter description new..." value={desc} onChange={handleDescChange} />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" disabled={adding} onClick={handleAdd}>
            {adding ? (
              <>
                <Spinner as="span" animation="grow" size="sm" role="status" aria-hidden="true" />
                Adding...
              </>
            ) : (
              <>Add Item</>
            )}
          </Button>
          <Button variant="secondary" onClick={handleClear}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  );
};

export default AddTodoItem;
