import { test, expect } from '@playwright/test';

test('Add a todo item and verify it appears in the list', async ({ page }) => {
  // 1. Go to the page
  await page.goto('http://localhost:3000/');

  const testDescription = 'UI Automation Test' + Date.now().toString();

  // 2. Fill out the input with the Id attribute "formAddTodoItem" with the value "UI Automation Test 1"
  await page.fill('#formAddTodoItem', testDescription);

  // 3. Click on the button with the label "Add Item"
  await page.click('button:has-text("Add Item")');

  // 5. Find the table by the id "todoitemlist" and check if it has a row containing the value "UI Automation Test 1"
  const todoListTable = await page.locator('#todoitemlist');
  const row = await todoListTable.locator('tr', { hasText: testDescription });

  // Assert that the row with the text exists in the table
  await expect(row).toBeVisible();
  expect(await row.count()).toBeGreaterThan(0);
});

test('Add todo item, mark it as completed, and verify the status', async ({ page }) => {
  // 1. Go to the page
  await page.goto('http://localhost:3000/');

  const testDescription = 'UI Automation Test' + Date.now().toString();
  // 2. Fill out the input with the Id attribute "formAddTodoItem" with the value "UI Automation Test 1"
  await page.fill('#formAddTodoItem', testDescription);

  // 3. Click on the button with the label "Add Item"
  await page.click('button:has-text("Add Item")');

  // 4. Find the table by the id "todoitemlist" and find the row with value "UI Automation Test 1"
  const todoListTable = await page.locator('#todoitemlist');
  const row = await todoListTable.locator('tr', { hasText: testDescription });

  await expect(row).toBeVisible();
  // Assert that the row with the text exists in the table
  expect(await row.count()).toBeGreaterThan(0);

  // 5. In the row we found in step 5, find the button with the text "Mark as completed", then click on the button.
  const markAsCompletedButton = row.locator('button:has-text("Mark as completed")');
  await markAsCompletedButton.click();

  const completedButton = row.locator('button:has-text("Completed")');
  await expect(completedButton).toBeVisible();
  expect(await completedButton.count()).toBeGreaterThan(0);
});
