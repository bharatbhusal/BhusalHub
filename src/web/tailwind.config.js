/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'class',
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        background: '#FFFFFF',
        foreground: '#334155',
        muted: '#F5F7FA',
        'muted-foreground': '#64748B',
        border: '#CBD5E1',
        primary: {
          DEFAULT: '#2563EB',
          foreground: '#FFFFFF',
        },
        navy: '#1E3A8A',
        accent: {
          DEFAULT: '#10B981',
          foreground: '#FFFFFF',
        },
        warning: '#F59E0B',
        destructive: {
          DEFAULT: '#EF4444',
          foreground: '#FFFFFF',
        },
      },
      borderRadius: {
        lg: '0.5rem',
        md: '0.375rem',
        sm: '0.25rem',
      },
    },
  },
  plugins: [require('tailwindcss-animate')],
};
