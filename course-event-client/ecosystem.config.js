module.exports = {
  apps: [
    {
      name: 'frontend',
      script: '.next/standalone/server.js',
      args: 'start',
      cwd: 'D:\\магістратура\\CourseEventsApi\\course-event-client',
      env: {
        NODE_ENV: 'production',
        PORT: 3000,
        HOSTNAME: '0.0.0.0',
      },
    },
  ],
};