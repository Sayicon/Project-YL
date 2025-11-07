/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   clean_ast.c                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/03 18:42:03 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/10 00:35:43 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	ast_free(t_ast *root)
{
	if (!root)
		return ;
	ast_free(root->left);
	ast_free(root->right);
	if (root->type == NODE_COMMAND && root->cmd)
		command_free(root->cmd);
	free(root);
}

void	command_free(t_command *command)
{
	if (command->heredoc_fd >= 0)
		close(command->heredoc_fd);
	if (command->args)
		free_args(command);
	if (command->redirs)
		free_redirs(command->redirs);
	free(command);
}

void	free_args(t_command *command)
{
	int	i;

	i = 0;
	while (i < command->arg_count)
		free(command->args[i++]);
	free(command->args);
}

void	free_redirs(t_redir *redir)
{
	t_redir	*next;

	while (redir)
	{
		next = redir->next;
		if (redir-> type == R_HEREDOC && redir->heredoc_fd >= 0)
			close(redir->heredoc_fd);
		free(redir->filename);
		free(redir);
		redir = next;
	}
}
