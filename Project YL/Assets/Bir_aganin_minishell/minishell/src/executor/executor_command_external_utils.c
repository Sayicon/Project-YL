/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_command_external_utils.c                  :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/13 04:01:44 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/16 00:27:11 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	is_explicit_dir(char *cmd)
{
	struct stat	sb;

	if (ft_strchr(cmd, '/') != NULL && stat(cmd, &sb) == 0
		&& S_ISDIR(sb.st_mode))
		return (1);
	return (0);
}

void	handle_dir_case(char *cmd, t_shell *shell)
{
	ft_putstr_fd("-minishell: ", STDERR_FILENO);
	ft_putstr_fd(cmd, STDERR_FILENO);
	ft_putstr_fd(": Is a directory\n", STDERR_FILENO);
	set_last_status(shell, 126);
	return ;
}

void	handle_not_found(char *cmd, t_shell *shell)
{
	ft_putstr_fd(cmd, STDERR_FILENO);
	if (ft_strchr(cmd, '/') != NULL)
		ft_putstr_fd(": No such file or directory\n", STDERR_FILENO);
	else
		ft_putstr_fd(": command not found\n", STDERR_FILENO);
	set_last_status(shell, 127);
}

void	handle_permission_denied(char *cmd, t_shell *shell)
{
	ft_putstr_fd("minishell: ", 2);
	ft_putstr_fd(cmd, 2);
	ft_putstr_fd(": Permission denied\n", 2);
	shell->last_status = 126;
}

int	exec_with_redirs(char **args, t_command *cmd, t_shell *shell)
{
	char	**envp;
	int		status;
	int		save_stdin;
	int		save_stdout;

	save_fds(&save_stdin, &save_stdout);
	if (apply_redirections(cmd, shell) < 0)
	{
		restore_fds(save_stdin, save_stdout);
		return (-1);
	}
	envp = env_list_to_array(shell->env);
	status = exec_dispatch(args, shell, envp);
	free_string_array(envp);
	restore_fds(save_stdin, save_stdout);
	set_last_status(shell, status);
	return (status);
}
