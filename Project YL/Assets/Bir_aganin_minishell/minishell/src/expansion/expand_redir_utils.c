/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expand_redir_utils.c                               :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/15 14:01:51 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/15 14:03:23 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	count_args(t_args *list)
{
	int	count;

	count = 0;
	while (list)
	{
		count++;
		list = list->next;
	}
	return (count);
}

void	handle_no_match(t_shell *sh, t_redir *redir)
{
	sh->last_status = 1;
	ft_putstr_fd("-minishell: ", STDERR_FILENO);
	ft_putstr_fd(redir->filename, STDERR_FILENO);
	ft_putstr_fd(": No such file or directory\n", STDERR_FILENO);
	redir->filename = NULL;
}

void	handle_ambiguous(t_shell *sh, t_redir *redir)
{
	sh->last_status = 1;
	ft_putstr_fd("-minishell: ", STDERR_FILENO);
	ft_putstr_fd(redir->filename, STDERR_FILENO);
	ft_putstr_fd(": ambiguous redirect\n", STDERR_FILENO);
	redir->filename = NULL;
}

void	handle_single_match(t_redir *redir, t_args *expanded_list)
{
	char	*old_filename;

	old_filename = redir->filename;
	redir->filename = ft_strdup(expanded_list->arg);
	free(old_filename);
}
