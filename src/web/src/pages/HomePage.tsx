import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '@/store/hooks';
import { logout } from '@/store/authSlice';
import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';
import { toast } from 'sonner';

export function HomePage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { user, token } = useAppSelector((s) => s.auth);
  const [secretMessage, setSecretMessage] = useState<string | null>(null);
  const [loadingMessage, setLoadingMessage] = useState(true);

  useEffect(() => {
    if (!token) return;
    fetch('/api/messages/secret', {
      headers: { 'Authorization': `Bearer ${token}` },
    })
      .then((r) => (r.ok ? r.json() : Promise.reject()))
      .then((data) => setSecretMessage(data.message))
      .catch(() => setSecretMessage('Failed to load secret message'))
      .finally(() => setLoadingMessage(false));
  }, [token]);

  const handleLogout = async () => {
    await dispatch(logout());
    toast.success('Logged out');
    navigate('/login', { replace: true });
  };

  return (
    <div className="min-h-screen bg-muted">
      <header className="border-b border-border bg-background">
        <div className="mx-auto flex max-w-4xl items-center justify-between p-4">
          <h1 className="text-xl font-semibold text-navy">BhusalHub</h1>
          <div className="flex items-center gap-4">
            <span className="text-sm text-muted-foreground">{user?.displayName}</span>
            <Button variant="outline" size="sm" onClick={handleLogout}>
              Sign out
            </Button>
          </div>
        </div>
      </header>

      <main className="mx-auto max-w-4xl space-y-4 p-4">
        <Card>
          <CardHeader>
            <CardTitle>Welcome, {user?.displayName}</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground">
              You are signed in as <strong>{user?.username}</strong>.
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Secret Message</CardTitle>
          </CardHeader>
          <CardContent>
            {loadingMessage ? (
              <div className="space-y-2">
                <Skeleton className="h-4 w-3/4" />
                <Skeleton className="h-4 w-1/2" />
              </div>
            ) : (
              <p className="text-foreground">{secretMessage}</p>
            )}
          </CardContent>
        </Card>
      </main>
    </div>
  );
}
