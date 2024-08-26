import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './tests', // Specify the test directory
  retries: 1, // Number of retries on failure
  use: {
    headless: true, // Run tests in headless mode
    viewport: { width: 1920, height: 1080 }, // Set the viewport size
    actionTimeout: 10000, // Timeout for each action
    baseURL: 'http://localhost:3000', // Base URL for your tests
  },
  webServer: {
    command: 'npm start', // Command to start your React app
    url: 'http://localhost:3000', // The URL to check if the app is running
    timeout: 120 * 1000, // Timeout to wait for the server to start
    reuseExistingServer: true,
  },
});
