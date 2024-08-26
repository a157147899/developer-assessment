import React from 'react';
import { Button, Table, Badge } from 'react-bootstrap';
import { TodoItem } from '../types';

interface TodoItemListProps {
  todoItems: TodoItem[];
  completeTodoItem: (item: TodoItem) => void;
  refresh: () => void;
}

const TodoItemList = ({ todoItems, completeTodoItem, refresh }: TodoItemListProps) => {
  return (
    <>
      <h1>
        Showing {todoItems.length} Item(s){' '}
        <Button variant="primary" className="pull-right" onClick={refresh}>
          Refresh
        </Button>
      </h1>

      <Table id="todoitemlist" striped bordered hover>
        <thead>
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {todoItems.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.description}</td>
              <td>
                {item.isCompleted ? (
                  <Badge bg="success">Completed</Badge>
                ) : (
                  <Button
                    variant="warning"
                    size="sm"
                    onClick={() => {
                      completeTodoItem(item);
                    }}
                  >
                    Mark as completed
                  </Button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  );
};

export default TodoItemList;
