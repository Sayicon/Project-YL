/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_dispatch.c                                :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/15 15:56:30 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/13 04:31:28 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	exec_dispatch(char **args, t_shell *shell, char **envp)
{
	struct termios	term_backup;
	int				status;
	pid_t			pid;

	if (isatty(STDIN_FILENO))
		tcgetattr(STDIN_FILENO, &term_backup);
	if (is_builtin(args[0]))
		return (run_builtin(args, shell));
	signal(SIGINT, handle_sigint_exec);
	signal(SIGQUIT, handle_sigquit_exec);
	pid = fork();
	if (pid < 0)
		exit_perror("fork failed");
	if (pid == 0)
	{
		setup_signals_exec();
		execve_with_path(args, shell->env, envp);
	}
	if (waitpid(pid, &status, 0) < 0)
		exit_perror("waitpid failed");
	if (isatty(STDIN_FILENO))
		tcsetattr(STDIN_FILENO, TCSANOW, &term_backup);
	setup_signals_prompt();
	return (WEXITSTATUS(status));
}

void	execve_with_path(char **args, t_env *env, char **envp)
{
	char	**dirs;
	char	*path_value;
	char	*candidate;
	int		i;

	try_exec_explicit(args[0], args, envp);
	path_value = get_env_path(env);
	dirs = ft_split(path_value, ':');
	if (!dirs)
		exit_perror("split PATH failed");
	i = 0;
	while (dirs[i])
	{
		candidate = ft_strjoin3(dirs[i], "/", args[0]);
		validate_exec_path(candidate);
		execve(candidate, args, envp);
		free(candidate);
		if (errno != ENOENT)
			perror("execve failed");
		i++;
	}
	free_string_array(dirs);
	perror("command not found");
	exit(127);
}

void	try_exec_explicit(char *cmd, char **args, char **envp)
{
	if (ft_strchr(cmd, '/') != NULL)
	{
		if (access(cmd, F_OK) == -1)
		{
			perror(cmd);
			free_string_array(envp);
			exit(127);
		}
		validate_exec_path(args[0]);
		execve(cmd, args, envp);
		perror("execve failed");
		exit(127);
	}
}

void	validate_exec_path(char *path)
{
	struct stat	sb;

	if (access(path, F_OK) == -1)
		return ;
	if (stat(path, &sb) == -1)
	{
		perror(path);
		exit(127);
	}
	if (S_ISDIR(sb.st_mode))
	{
		write(2, "-minishell: ", 12);
		write(2, path, ft_strlen(path));
		write(2, ": Is a directory\n", 17);
		exit(126);
	}
	if (access(path, X_OK) == -1)
	{
		perror(path);
		exit(126);
	}
}
