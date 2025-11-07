/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ast.c                                              :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/29 16:50:15 by marcudos          #+#    #+#             */
/*   Updated: 2025/04/30 20:32:20 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

t_ast	*parse_logical(t_token **token, t_shell *shell)
{
	t_ast	*left;
	t_ast	*right;
	t_ast	*new;
	t_token	*t;
	int		type;

	t = (*token);
	left = parse_pipe(&t, shell);
	while (t && (t->type == LOGICAL_OR || t->type == LOGICAL_AND))
	{
		type = t->type;
		t = t->next;
		right = parse_pipe(&t, shell);
		new = (t_ast *) malloc(sizeof(t_ast));
		new->left = left;
		new->right = right;
		new->cmd = NULL;
		if (type == LOGICAL_OR)
			new->type = NODE_OR;
		else
			new->type = NODE_AND;
		left = new;
	}
	return (left);
}

t_ast	*parse_pipe(t_token **token, t_shell *shell)
{
	t_ast	*left;
	t_ast	*right;
	t_ast	*new;
	t_token	*t;

	t = (*token);
	left = parse_simple_command(&t, shell);
	while (t && t->type == PIPE)
	{
		t = t->next;
		right = parse_simple_command(&t, shell);
		new = (t_ast *) malloc(sizeof(t_ast));
		new->left = left;
		new->right = right;
		new->cmd = NULL;
		new->type = NODE_PIPE;
		left = new;
	}
	(*token) = t;
	return (left);
}

t_ast	*parse_simple_command(t_token **token, t_shell *shell)
{
	if ((*token)->type == PAREN_OPEN)
		return (parse_subshell(token, shell));
	return (parse_command(token, shell));
}

t_ast	*parse_subshell(t_token **token, t_shell *shell)
{
	t_ast	*subtree;
	t_ast	*node;
	t_token	*t;
	int		nesting_level;

	t = (*token);
	if (!t || t->type != PAREN_OPEN)
		return (NULL);
	t = t->next;
	subtree = parse_logical(&t, shell);
	nesting_level = 1;
	while (t && nesting_level > 0)
	{
		if (t->type == PAREN_OPEN)
			nesting_level++;
		else if (t->type == PAREN_CLOSE)
			nesting_level--;
		t = t->next;
	}
	node = (t_ast *) malloc(sizeof(t_ast));
	node->left = subtree;
	node->right = NULL;
	node->cmd = NULL;
	node->type = NODE_SUBSHELL;
	return ((*token) = t, node);
}

t_ast	*parse_command(t_token **token, t_shell *shell)
{
	t_ast	*new;

	new = (t_ast *) malloc(sizeof(t_ast));
	new->left = NULL;
	new->right = NULL;
	new->type = NODE_COMMAND;
	new->cmd = make_command(token, shell);
	return (new);
}
