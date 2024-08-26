import './App.css';
import { Image, Alert, Button, Container, Row, Col, Form, Table, Stack } from 'react-bootstrap';
import React, { useState, useEffect } from 'react';
import { TodoItem } from './types';
import AddTodoItem from './components/AddTodoItem';
import TodoItemList from './components/TodoItemList';
import todoItemService from './services/TodoItemService';
import { AxiosError } from 'axios';

const App = () => {
  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
  const [error, setError] = useState({ visible: false, message: '' });

  useEffect(() => {
    handleRefreshTodoItems();
  }, []);

  const showUserFriendlyError = (error: AxiosError) => {
    if (error?.response?.data) {
      setError({ visible: true, message: JSON.stringify(error.response.data) });
    } else if (error.message.includes('Network Error')) {
      setError({ visible: true, message: 'Network issue. Please make sure the API is running.' });
    } else {
      setError({ visible: true, message: 'Unknown error, please try it later or contact support.' });
    }
  };

  const handleSetError = (error: string) => {
    setError({ visible: true, message: error });
  };

  const handleCloseAlert = () => {
    setError({ visible: false, message: '' });
  };

  const handleRefreshTodoItems = () => {
    const { request, cancel } = todoItemService.getAll<TodoItem>();

    request
      .then((res) => {
        setTodoItems(res.data);
      })
      .catch((error) => {
        cancel();
      });
  };

  const handleAddItem = (desc: string, callback: (success: boolean) => void) => {
    var newTodoItem = { description: desc, isCompleted: false };
    todoItemService
      .create<TodoItem>(newTodoItem)
      .then((res) => {
        setTodoItems([res.data, ...todoItems]);
        handleCloseAlert();
        callback(true);
      })
      .catch((error) => {
        showUserFriendlyError(error);
        callback(false);
      });
  };

  const handleCompleteTodoItem = (todoItem: TodoItem) => {
    var updatedTodoItem = { ...todoItem, isCompleted: true };
    todoItemService
      .update<TodoItem>(updatedTodoItem)
      .then((res) => {
        setTodoItems(todoItems.map((item) => (item.id === todoItem.id ? updatedTodoItem : item)));
      })
      .catch((error) => {
        showUserFriendlyError(error);
      });
  };

  return (
    <div className="App">
      <Container>
        <Row>
          <Col>
            <Image src="clearPointLogo.png" fluid rounded />
          </Col>
        </Row>
        <Row>
          <Col>
            {error.visible && (
              <Alert variant="danger" onClose={handleCloseAlert} dismissible>
                {error.message}
              </Alert>
            )}
          </Col>
        </Row>
        <Row>
          <Col>
            <AddTodoItem addTodoItem={handleAddItem} showError={handleSetError}></AddTodoItem>
          </Col>
        </Row>
        <br />
        <Row>
          <Col>
            <TodoItemList
              todoItems={todoItems}
              completeTodoItem={handleCompleteTodoItem}
              refresh={handleRefreshTodoItems}
            ></TodoItemList>
          </Col>
        </Row>
      </Container>
      <footer className="page-footer font-small teal pt-4">
        <div className="footer-copyright text-center py-3">
          © 2021 Copyright:
          <a href="https://clearpoint.digital" target="_blank" rel="noreferrer">
            clearpoint.digital
          </a>
        </div>
      </footer>
    </div>
  );
};

export default App;
