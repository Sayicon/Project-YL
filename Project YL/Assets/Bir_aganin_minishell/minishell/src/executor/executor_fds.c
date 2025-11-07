/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_fds.c                                     :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/14 16:04:54 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/14 16:05:58 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	save_fds(int *save_stdin, int *save_stdout)
{
	*save_stdin = dup(STDIN_FILENO);
	*save_stdout = dup(STDOUT_FILENO);
	if (*save_stdin < 0 || *save_stdout < 0)
		exit_perror("dup failed");
	return (0);
}

int	restore_fds(int save_stdin, int save_stdout)
{
	if (dup2(save_stdin, STDIN_FILENO) < 0
		|| dup2(save_stdout, STDOUT_FILENO) < 0)
		exit_perror("dup2 restore failed");
	close(save_stdin);
	close(save_stdout);
	return (0);
}
