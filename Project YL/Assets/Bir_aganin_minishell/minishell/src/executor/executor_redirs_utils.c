/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_redirs_utils.c                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/10 00:51:34 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/10 00:51:52 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	handle_in_redir(t_redir *redir, int *in_fd, t_shell *sh)
{
	int	fd;

	if (redir->filename == NULL)
		return (-1);
	fd = open_file_err(redir->filename, O_RDONLY, 0, sh);
	if (fd < 0)
		return (-1);
	if (*in_fd != -1)
		close(*in_fd);
	*in_fd = fd;
	return (0);
}

int	open_file_err(char *filename, int flags, int mode, t_shell *sh)
{
	int	fd;

	fd = open(filename, flags, mode);
	if (fd < 0)
	{
		ft_putstr_fd("-minishell: ", STDERR_FILENO);
		ft_putstr_fd(filename, STDERR_FILENO);
		ft_putstr_fd(": ", STDERR_FILENO);
		ft_putstr_fd(strerror(errno), STDERR_FILENO);
		ft_putstr_fd("\n", STDERR_FILENO);
		sh->last_status = 1;
	}
	return (fd);
}

int	handle_heredoc(t_redir *redir, int *in_fd)
{
	if (*in_fd != -1)
		close(*in_fd);
	*in_fd = redir->heredoc_fd;
	return (0);
}

int	handle_out_redir(t_redir *redir, int *out_fd, t_shell *sh)
{
	int	flags;
	int	fd;

	if (redir->filename == NULL)
		return (-1);
	flags = O_CREAT | O_WRONLY;
	if (redir->type == R_OUT)
		flags |= O_TRUNC;
	else
		flags |= O_APPEND;
	fd = open_file_err(redir->filename, flags, 0644, sh);
	if (fd < 0)
		return (-1);
	if (*out_fd != -1)
		close(*out_fd);
	*out_fd = fd;
	return (0);
}

void	dup2_close(int fd, int std_fd)
{
	dup2(fd, std_fd);
	close(fd);
}
