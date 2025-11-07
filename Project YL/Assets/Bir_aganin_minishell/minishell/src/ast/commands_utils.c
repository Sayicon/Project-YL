/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   commands_utils.c                                   :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/10 00:23:33 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/10 00:33:39 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	parse_heredoc(t_token **token, t_command **cmd, t_shell *shell)
{
	t_redir	*redir;

	if (g_signal_status == -1)
	{
		redir = create_heredoc_node((*token)->next->value, shell);
		build_redir_list(&(*cmd)->redirs, redir);
	}
	(*token) = (*token)->next->next;
}

t_redir	*create_heredoc_node(char *raw, t_shell *shell)
{
	int		no_expand;
	char	*delim;
	int		fd;
	t_redir	*redir;

	delim = strip_quotes(raw, &no_expand);
	fd = process_hd(delim, no_expand, shell->env, shell);
	redir = (t_redir *)malloc(sizeof(*redir));
	if (!redir)
		exit_perror("malloc");
	redir->type = R_HEREDOC;
	redir->filename = delim;
	redir->heredoc_fd = fd;
	redir->next = NULL;
	return (redir);
}

char	*strip_quotes(char *raw, int *no_expand)
{
	*no_expand = have_quotes(raw);
	return (rem_quotes(ft_strdup(raw), 1));
}

void	build_redir_list(t_redir **head, t_redir *node)
{
	t_redir	*iter;

	if (!*head)
		*head = node;
	else
	{
		iter = *head;
		while (iter->next)
			iter = iter->next;
		iter->next = node;
	}
}

void	append_redir(t_redir **head, t_redir_type type, char *filename)
{
	t_redir	*redir;

	redir = (t_redir *)malloc(sizeof(*redir));
	if (!redir)
		exit_perror("malloc");
	redir->type = type;
	redir->filename = rem_quotes(ft_strdup(filename), 1);
	redir->heredoc_fd = -1;
	redir->next = NULL;
	build_redir_list(head, redir);
}
